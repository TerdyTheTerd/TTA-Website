$(document).ready(function () {
    var wbbOpt = {
        buttons: "bold,italic,underline,,strike,sup,sub,fontcolor,fontsize,|,table,img,link,video,|,code,quote,spoiler,bullist,numlist,|,justifyleft,justifycenter,justifyright,removeFormat"
    }
    $("#editor").wysibb(wbbOpt);
    initMessage();

    function initMessage() {
        var currentView = 'inboxText';
        $("#" + currentView).css("color", "black");
        $('#messageCompose').on('click', function () {
            $("#" + currentView).css("color", "grey");
            $("#composeText").css("color", "black");
            currentView = 'composeText';
        });
        $('#messageInbox').on('click', function () {
            $("#" + currentView).css("color", "grey");
            $("#inboxText").css("color", "black");
            currentView = 'inboxText';
        });
        $('#messageSend').on('click', function () {
            $("#" + currentView).css("color", "grey");
            $("#sendText").css("color", "black");
            currentView = 'sendText';
        });
        $('#messageDrafts').on('click', function () {
            $("#" + currentView).css("color", "grey");
            $("#draftsText").css("color", "black");
            currentView = 'draftsText';
        });
        if (view === currentView) {
            $("#" + currentView).css("color", "grey");
        }
    };

    $("#messagePreview").on("click", function () {
        var previewBody = document.getElementById('hurr').innerHTML;
        $.ajax({
            url: "/Forums/PreviewPost",
            type: 'POST',
            dataType: 'html',
            data: { test: $('#hurr').html() },
            success: function (data) {
                $("#previewHolder").css('display', 'block');
                $("#previewHolder").html(data);
            },
            error: function (xhr, status) {
                alert("Sorry, there was a problem!");
            }
        });
    });


    $('#messageSubmit').on("click", function () {
        //First get the userlist and set the hidden value to it
        var list = $('#listInput').val();
        $('#userList').val(list);

        $('#messageForm').submit();
        //Submit the form to post data to the controller, upon validation it will return the user to view the message

    });
    //Create a function that binds to userlist input area, then Bidns a timeout to keypress event is its focused on
    //Restart timeout if another key is pressed, then if timeout is reached perform an ajax request to get suggestions
    //that the user can click on to finish or correct the spelling of the username

    //Create function to bind to page number and request the next x results once clicked, then replace the content in the side
    //div with the new data
});