using System;
using System.Collections.Generic;
using System.Diagnostics;

using Autofac;

using TRock.Music;
using TRock.Music.Grooveshark;
using TRock.Music.Spotify;
using TRock.Music.Torshify;

namespace TRock.Party
{
    public class SiteModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            Trace.Listeners.Add(new ConsoleTraceListener());

            if (SiteGlobal.GroovesharkEnabled)
            {
                // Grooveshark
                builder.RegisterType<GroovesharkClientWrapper>().As<IGroovesharkClient>().OnActivating(a => a.Instance.Connect()).SingleInstance();
                builder.RegisterType<GroovesharkSongProvider>().Named<ISongProvider>("SongProvider");
                builder.RegisterType<GroovesharkSongPlayer>().Named<ISongPlayer>("SongPlayer");
            }

            if (SiteGlobal.SpotifyEnabled && (!string.IsNullOrEmpty(SiteGlobal.SpotifyUsername) && !string.IsNullOrEmpty(SiteGlobal.SpotifyPassword)))
            {
                // Spotify
                builder.RegisterType<SpotifySongProvider>().Named<ISongProvider>("SongProvider");
                builder.Register(ctx => new TorshifySongPlayerClient(new Uri("http://localhost:8081")))
                    .Named<ISongPlayer>("SongPlayer").OnActivating(a => a.Instance.Connect());
                builder.RegisterType<TorshifyImageProvider>().As<ISpotifyImageProvider>()
                    .OnActivating(a => a.Instance.TorshifyServerUrl = "http://localhost:8082").SingleInstance();
                builder.RegisterType<TorshifyServerProcessStarter>().As<IStartable>();
            }

            // Aggregate
            builder.Register(ctx =>
            {
                var a = new AggregateSongProvider(ctx.ResolveNamed<IEnumerable<ISongProvider>>("SongProvider"));
                a.UnhandledException += (sender, args) =>
                {
                    Trace.WriteLine(args.Exception);
                    args.Handled = true;
                };
                return a;
            }).Named<ISongProvider>("Aggregate").SingleInstance();
            builder.Register(ctx => new AggregateSongPlayer(ctx.ResolveNamed<IEnumerable<ISongPlayer>>("SongPlayer"))).As<ISongPlayer>().SingleInstance();

            // Cached song provider
            builder.Register(ctx => new CachedSongProvider(ctx.ResolveNamed<ISongProvider>("Aggregate")) { SlidingExpiration = TimeSpan.FromMinutes(5) }).As<ISongProvider>().SingleInstance();

            // Other servies
            builder.RegisterType<VoteableQueue<ISongStream>>().As<IVoteableQueue<ISongStream>>().SingleInstance();
            builder.RegisterType<SongStreamPlayer>().As<ISongStreamPlayer>().SingleInstance();
            builder.RegisterType<SongStreamHandler>().As<IStartable>();
            builder.RegisterType<QueueEventPublisher>().As<IStartable>();
            builder.RegisterType<PlayerEventPublisher>().As<IStartable>();
            builder.RegisterType<AutoPlaylistService>().As<IStartable>();
            builder.RegisterType<SongHistoryService>().As<ISongHistoryService>().As<IStartable>().SingleInstance();
        }
    }
}