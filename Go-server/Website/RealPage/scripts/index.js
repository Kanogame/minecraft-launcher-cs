let slideIndex = 1;
showSlides(slideIndex);

const prev = document.querySelector(".prev");
prev.addEventListener("click", () => {
  plusSlides(-1);
});

const next = document.querySelector(".next");
next.addEventListener("click", () => {
  plusSlides(1);
});

function plusSlides(n) {
  showSlides(slideIndex += n);
}

function currentSlide(n) {
  showSlides(slideIndex = n);
}

function showSlides(n) {
  let i;
  let slides = document.getElementsByClassName("slide");
  if (n > slides.length) {slideIndex = 1}
  if (n < 1) {slideIndex = slides.length}
  for (i = 0; i < slides.length; i++) {
    slides[i].style.display = "none";
  }
  slides[slideIndex-1].style.display = "block";
}