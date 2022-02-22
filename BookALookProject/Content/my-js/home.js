const slideshow = document.querySelector(".image-slideshow");
var imgNum = 1;

const interval = setInterval(function () {
    if(imgNum > 3)
        imgNum = 1;
    console.log(imgNum);

    slideshow.style.backgroundImage="url(/Content/images/picture" + imgNum + ".jpg)";
    imgNum++;
}, 7000);





