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
    $("#upload").change(function (event) {
        $("#imagestart").hide();
        //Create the image
        var img = document.createElement("img");
        img.src = URL.createObjectURL(event.target.files[0]);
        img.setAttribute("id", "image");
        img.setAttribute("position", "relative");
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
                    aspectRatio: 5.1428
                });
            } else if (Type === 'profile') {
                $("#image").imageCrop({
                    overlayOpacity: 0.25,
                    displayPreview: true,
                    displaySizeHint: true,
                    imageType: Type,
                    onSelect: updateForm,
                    aspectRatio: 1
                });
            }


        };
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
}