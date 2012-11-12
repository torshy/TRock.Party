/* Events
-------------------------------------------------- */
var hubEvents = new function () {
    var hub = $.connection.mainHub;
    var queueChangedListeners = [];
    var queueItemAddedListeners = [];
    var songChangedListeners = [];
    var isPlayingChangedListeners = [];
    var progressListeners = [];
    var bufferingListeners = [];

    hub.queueChanged = function (data) {
        for (var i = 0; i < queueChangedListeners.length; i++) {
            queueChangedListeners[i](data);
        }
    };
    
    hub.queueItemAdded = function (data) {
        for (var i = 0; i < queueItemAddedListeners.length; i++) {
            queueItemAddedListeners[i](data);
        }
    };
    
    hub.songChanged = function (data) {
        for (var i = 0; i < songChangedListeners.length; i++) {
            songChangedListeners[i](data);
        }
    };
    
    hub.isPlayingChanged = function (data) {
        for (var i = 0; i < isPlayingChangedListeners.length; i++) {
            isPlayingChangedListeners[i](data);
        }
    };
    
    hub.progress = function (data) {
        for (var i = 0; i < progressListeners.length; i++) {
            progressListeners[i](data);
        }
    };
    
    hub.buffering = function (data) {
        for (var i = 0; i < bufferingListeners.length; i++) {
            bufferingListeners[i](data);
        }
    };
    
    this.addQueueChangedListener = function (listener) {
        queueChangedListeners.push(listener);
    };

    this.addQueueItemAddedListener = function(listener) {
        queueItemAddedListeners.push(listener);
    };

    this.addSongChangedListener = function(listener) {
        songChangedListeners.push(listener);
    };
    
    this.addIsPlayingChangedListener = function (listener) {
        isPlayingChangedListeners.push(listener);
    };
    
    this.addProgressListener = function (listener) {
        progressListeners.push(listener);
    };
    
    this.addBufferingListener = function (listener) {
        bufferingListeners.push(listener);
    };
};

/* Voting
-------------------------------------------------- */

function upvoteById(queueId) {
    $.post('/api/upvote/' + queueId);
}

function upvote(queueItem) {
    $.post('/api/upvote/' + queueItem.Id);
}

function downvoteById(queueId) {
    $.post('/api/downvote/' + queueId);
}

function downvote(queueItem) {
    $.post('/api/downvote/' + queueItem.Id);
}

function moveToNext() {
    $.post('/services/queue/movetonext');
}

/* Player
-------------------------------------------------- */

$('.play').click(function () {
    $.post('/services/player/play');
});

$('.pause').click(function () {
    $.post('/services/player/pause');
});

$('.stop').click(function () {
    $.post('/services/player/stop');
});

$('.volume-up').click(function () {
    $.post('/services/player/volumeup');
});

$('.volume-down').click(function () {
    $.post('/services/player/volumedown');
});

$("html").on("keydown", function (e) {
    var element = e.target.nodeName.toLowerCase();
    if ((element != 'input' && element != 'textarea') || $(e.target).attr("readonly")) {
        var code = (e.keyCode ? e.keyCode : e.which);
        // Spacebar
        if (code === 32) {
            $.post('/services/player/toggleplay');
            e.preventDefault();
            return false;
        }
            // + sign
        else if (code == 107) {
            $.post('/services/player/volumeup');
            e.preventDefault();
            return false;
        }
            // - sign
        else if (code == 109) {
            $.post('/services/player/volumedown');
            e.preventDefault();
            return false;
        }
    }

    return true;
});