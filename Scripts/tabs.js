var tabLinks;
var tabPanels;
window.onload = function () {
    //Initial state
    tabLinks = document.getElementById("tabs").getElementsByTagName("li"); //Get "li" elements from "tabs" element
    tabPanels = document.getElementById("containers").getElementsByClassName("tab");
    displayPanel(tabLinks[0]); //Set initial tab display as first element
    //Add an onClick event listener to each element in tabLinks
    for (var i = 0; i < tabLinks.length; i++) {
        tabLinks[i].onclick = function () {
            displayPanel(this);
            return false;
        }
        //For keyboards or other devices
        tabLinks[i].onfocus = function () {
            displayPanel(this);
            return false;
        }
    }
}
function displayPanel(tabToActivate) {
    for (var i = 0; i < tabLinks.length; i++) {
        if (tabLinks[i] == tabToActivate) {
            tabLinks[i].classList.add("active");
            tabPanels[i].style.display = "block";
        } else {
            tabLinks[i].classList.remove("active");
            tabPanels[i].style.display = "none";
        }
    }
}
$(document).ready(function () {
    NotifyNew();
    function NotifyNew() {
        if (localStorage.getItem('notifyState') != 'show') {
            $('#Notify').show();
            localStorage.setItem('notifyState', 'show')
        }
        $('#notifybutton').on('click', function () {
            $('#Notify').hide();
        })
    };
    $('.pics').magnificPopup({
        type: 'image',
        closeOnContentClick: true,
        closeBtnInside: false,
        image: {
            verticalFit: true
        },
        mainClass: 'mfp-no-margins mfp-with-zoom',
        zoom: {
            enabled: true,
            duration: 300 // don't foget to change the duration also in CSS
        },
        gallery: { enabled: true },
        callbacks: {
            buildControls: function () {
                // re-appends controls inside the main container
                this.contentContainer.append(this.arrowLeft.add(this.arrowRight));
            }
        }
    });
    $('.toggle').show();
    $('a.togglelink').on('click', function (e) {
        $("div.toggle").toggle(1000);
    });

    $('.toggle1').show();
    $('a.togglelink1').on('click', function (e) {
        $("div.toggle1").toggle(1000);
    });

    $('.toggle2').show();
    $('a.togglelink2').on('click', function (e) {
        $("div.toggle2").toggle(1000);
    });

    $('.toggle3').show();
    $('a.togglelink3').on('click', function (e) {
        $("div.toggle3").toggle(1000);
    });


    $("#country").countrySelect({
        preferredCountries: ["us", "gb", "au"],
        responsiveDropdown: false
    });

    $("#Effects").change(function () {
        alert("It worked");
        $("#effectPreview").css("background-image", this.value);
    });
});

function imagewrapper(Type) {
    var isImageLoaded = false;
    $("#cancel").on("click", function () {
        $("#imageuploader").empty();
        $("#imageseperator").hide();
        resizeToDefault();
    });
    $(".imageclose").on("click", function () {
        $("#imageuploader").empty();
        $("#imageseperator").hide();
        resizeToDefault();
    });
    $("#imagedelete").on("click", function () {
        $("#imagemain").empty();
        $("#imagestart").show();
    });
    var _URL = window.URL || window.webkitURL;
    $("#img").change(function (e) {
        $("#imagestart").hide();
        $("#imagedelete").show();
        //Create the image, Check if image is too large, ask user if they would like to manually resize or let the server automatically resize
        var file, pic, sr;
        if ((file = this.files[0])) {
            pic = new Image();
            pic.onload = function () {
                alert(this.width + " " + this.height);
                var img = document.createElement("img");
                img.src = URL.createObjectURL(e.target.files[0]);
                img.setAttribute("id", "image");
                img.setAttribute("position", "relative");
                if (pic.width > $(window).width()) {
                    var y = Math.round($(window).width() * .95); //Rough Value to account for padding needed, should be changed to an exact value
                    var x = y / pic.width;
                    sr = x;
                    img.setAttribute("width", Math.round(pic.width * x));
                } else {
                    sr = y = 1;
                }
                document.getElementById("imagemain").appendChild(img);
                isImageLoaded = true;
                if (isImageLoaded) {
                    if (Type === 'banner') {
                        $("#image").imageCrop({
                            overlayOpacity: 0.25,
                            displayPreview: true,
                            displaySizeHint: true,
                            imageType: Type,
                            onSelect: updateForm,
                            aspectRatio: 5.1428,
                            scaleRatio: sr
                        });
                    } else if (Type === 'profile') {
                        $("#image").imageCrop({
                            overlayOpacity: 0.25,
                            displayPreview: true,
                            displaySizeHint: true,
                            imageType: Type,
                            onSelect: updateForm,
                            aspectRatio: 1,
                            scaleRatio: sr
                        });
                    }
                };
            };
            pic.src = _URL.createObjectURL(file);
        }
    });
    //Create binding event when mousepressdown and mousemove on #image it sets the absolute position of imageselection from current mouse location
    //$("#imagemain").on("click", function () {
    //   $("#imageoverlay").toggle();

    // });
    $("#savebanner").on("click", function () {
        var img = document.getElementById("imageform");
        img.submit();
    });
}

function resizeToDefault() {
    $("#imageseperator").css("height", $(window).height() + "px")
}
function updateForm(cropData) {
    $('#offSetX').val(cropData.selectionX);
    $('#offSetY').val(cropData.selectionY);
    $('#width').val(cropData.selectionWidth);
    $('#height').val(cropData.selectionHeight);
    $('#type').val(cropData.imageType);
    $('#scaleRatio').val(cropData.scaleRatio);
}
//Change into a more simplified logic loop to reduce code used, collect elements, then when one is clicked take this, apply active status, and set all others to inactive
function initDash(view) {
    var currentView = view;
    $("#" + currentView).css("color", "grey");
    $("#activity").on("click", function () {
        $("#" + currentView).css("color", "#333");
        $("#activity").css("color", "grey");
        currentView = 'activity';
    });
    $("#info").on("click", function () {
        $("#" + currentView).css("color", "#333");
        $("#info").css("color", "grey");
        currentView = 'info';
    });
    $("#pics").on("click", function () {
        $("#" + currentView).css("color", "#333");
        $("#pics").css("color", "grey");
        currentView = 'pics';
    });
    $("#post").on("click", function () {
        $("#" + currentView).css("color", "#333");
        $("#post").css("color", "grey");
        currentView = 'post';
    });
    if (view === currentView) {
        $("#" + currentView).css("color", "grey");
    }
    $("#option").on("click", function () {
        var hurr = document.getElementById('addFriend');
        $("#id").val('@model.Id')
        hurr.submit();
    })
}
function getUserLevel() {
    $.ajax({
        url: "/Dashboard/UserLevel",
        type: 'POST',
        dataType: 'html',
        data: { name: Model.User.ApplicationUserId },
        success: function (data) {
            alert("It Worked");
        },
        error: function (xhr, status) {
            alert("There was an error");
        }
    });
}

function calcLevel(userExp, levelExp, prevExp) {
    var currentExp = levelExp - userExp; //Get difference so we can show progress from the current level to the next
    $("#userLevelBarL").css({ backgroundColor: "green", display: "inline" });
    
    var percBar = ((userExp - prevExp)* 100 ) / (levelExp - prevExp);
    $("#userLevelBarL").width(percBar + "%");
}