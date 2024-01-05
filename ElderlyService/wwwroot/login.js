/*
		Designed by: SELECTO
		Original image: https://dribbble.com/shots/5311359-Diprella-Login
*/

let switchCtn = document.querySelector("#switch-cnt");
let switchC1 = document.querySelector("#switch-c1");
let switchC2 = document.querySelector("#switch-c2");
let switchCircle = document.querySelectorAll(".switch__circle");
let switchBtn = document.querySelectorAll(".switch-btn");
let aContainer = document.querySelector("#a-container");
let bContainer = document.querySelector("#b-container");
let allButtons = document.querySelectorAll(".submit");

let getButtons = (e) => e.preventDefault();

let changeForm = (e) => {
	switchCtn.classList.add("is-gx");
	setTimeout(function () {
		switchCtn.classList.remove("is-gx");
	}, 1500);

	switchCtn.classList.toggle("is-txr");
	switchCircle[0].classList.toggle("is-txr");
	switchCircle[1].classList.toggle("is-txr");

	switchC1.classList.toggle("is-hidden");
	switchC2.classList.toggle("is-hidden");
	aContainer.classList.toggle("is-txl");
	bContainer.classList.toggle("is-txl");
	bContainer.classList.toggle("is-z200");
};

let mainF = (e) => {
	for (var i = 0; i < allButtons.length; i++)
		allButtons[i].addEventListener("click", getButtons);
	for (var i = 0; i < switchBtn.length; i++)
		switchBtn[i].addEventListener("click", changeForm);
};

window.addEventListener("load", mainF);

function validateForm() {
	var name = document.getElementById("Name").value;
	var email = document.getElementById("Email").value;
	var password = document.getElementById("Password").value;

	var nameError = document.getElementById("nameError");
	var emailError = document.getElementById("emailError");
	var passwordError = document.getElementById("passwordError");

	// Reset previous errors
	nameError.innerHTML = "";
	emailError.innerHTML = "";
	passwordError.innerHTML = "";

	var isValid = true;

	// Validate Name
	if (name.trim() === "") {
		nameError.innerHTML = "Name is required";
		isValid = false;
	}

	// Validate Email
	if (!isValidEmail(email)) {
		emailError.innerHTML = "Invalid email address";
		isValid = false;
	}

	// Validate Password
	if (password.length < 8) {
		passwordError.innerHTML = "Password must be at least 8 characters long";
		isValid = false;
	} else if (!containsUpperCase(password)) {
		// // passwordError.innerHTML = "";
		passwordError.innerHTML = "Password must contain at least one uppercase letter";
		isValid = false;
	} else if (!containsLowerCase(password)) {
		// passwordError.innerHTML = "";
		passwordError.innerHTML = "Password must contain at least one lowercase letter";
		isValid = false;
	} else if (!containsDigit(password)) {
		// passwordError.innerHTML = "";
		passwordError.innerHTML = "Password must contain at least one digit";
		isValid = false;
	}

	return isValid;
}

function isValidEmail(email) {
	// Regular expression for a valid email address
	var emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
	return emailRegex.test(email);
}

function containsUpperCase(str) {
	return /[A-Z]/.test(str);
}

function containsLowerCase(str) {
	return /[a-z]/.test(str);
}

function containsDigit(str) {
	return /\d/.test(str);
}