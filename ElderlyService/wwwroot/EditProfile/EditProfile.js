


function validateForm() {
    var fname = document.getElementById("fName").value;
    var lname = document.getElementById("lName").value;
    var email = document.getElementById("Email").value;
    var password = document.getElementById("Password").value;
    var phoneNumber = document.getElementById("Phone").value;
    var locationUrl = document.getElementById("Location").value;

    var nameError = document.getElementById("nameError");
    var emailError = document.getElementById("emailError");
    var passwordError = document.getElementById("passwordError");
    var phoneError = document.getElementById("phoneError");
    var locationError = document.getElementById("locationError");

    // Reset previous errors
    nameError.innerHTML = "";
    emailError.innerHTML = "";
    passwordError.innerHTML = "";
    phoneError.innerHTML = "";
    locationError.innerHTML = "";

    var isValid = true;

    // Validate Name
    if (fname.trim() === "") {
        nameError.innerHTML = "Name is required";
        isValid = false;
    }
    if (lname.trim() === "") {
        nameError.innerHTML = "Name is required";
        isValid = false;
    }

    // Validate Email
    if (!isValidEmail(email)) {
        emailError.innerHTML = "Invalid email address";
        isValid = false;
    }

    // Validate Phone Number
    if (phoneNumber.trim() === "") {
        phoneError.innerHTML = "Phone number is required";
        isValid = false;
    } else if (!isValidPhoneNumber(phoneNumber)) {
        phoneError.innerHTML = 'Invalid phone number format (e.g., 1234567890)';
        isValid = false;
    }

    // Validate Location URL
    if (locationUrl.trim() === "") {
        locationError.innerHTML = "Location URL is required";
        isValid = false;
    } else if (!isValidUrl(locationUrl)) {
        locationError.innerHTML = 'Invalid URL format';
        isValid = false;
    }

    // Validate Password
    if (password.length < 8) {
        passwordError.innerHTML = "Password must be at least 8 characters long";
        isValid = false;
    } else if (!containsUpperCase(password)) {
        passwordError.innerHTML = "Password must contain at least one uppercase letter";
        isValid = false;
    } else if (!containsLowerCase(password)) {
        passwordError.innerHTML = "Password must contain at least one lowercase letter";
        isValid = false;
    } else if (!containsDigit(password)) {
        passwordError.innerHTML = "Password must contain at least one digit";
        isValid = false;
    }

    return isValid;
}

function isValidEmail(email) {
    // Regular expression for a valid email address
    var emailRegex = /^[^\s@@]+@@[^\s@@]+\.[^\s@@]+$/;
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

function isValidUrl(url) {
    var urlRegex = /^(http|https):\/\/[^ "]+$/;
    return urlRegex.test(url);
}

function isValidPhoneNumber(phoneValue) {
    var phoneRegex = /^[0-9]{10}$/;
    return phoneRegex.test(phoneValue)
}


