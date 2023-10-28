function onLogoFileUpload(elementId) {
	return function (e) {
		let targetImage = document.getElementById(`${elementId}Preview`);
		let logoInput = document.getElementById(elementId);

		const [file] = logoInput.files

		if (file) {
			targetImage.src = URL.createObjectURL(file);
			targetImage.classList.remove('hidden');
		} else {
			targetImage.classList.add('hidden');
		}
	};
};


function setLanguageCookie(lang, expDays) {
	// Default at 365 days.
	days = expDays || 365;

	// Get unix milliseconds at current time plus number of days
	var date = new Date();
	date.setTime(+date + (days * 86400000)); //24 * 60 * 60 * 1000
	const expires = "expires=" + date;

	document.cookie = `.AspNetCore.Culture=c=${lang}|uic=${lang}; ${expires}; path=/; SameSite=Lax`;
}

function getLanguageCookie() {
	const name = ".AspNetCore.Culture=";
	const cDecoded = decodeURIComponent(document.cookie); // to be careful
	const cArr = cDecoded.split('; ');
	let res;
	cArr.forEach(val => {
		if (val.indexOf(name) === 0) {
			res = val.substring(name.length);
		}
	});
	return res;
}

function getCurrentLanguage() {
	let currentLanguage = getLanguageCookie();
	let result = currentLanguage;
	if (currentLanguage) {
		var split = currentLanguage.split('=');
		result = split[split.length - 1];
	}
	return result;
}

function setLanguage(newLang) {
	setLanguageCookie(newLang);
	location.reload(true);
}

function toggleLanguage() {
	let language = getCurrentLanguage();

	if (language === 'en') {
		setLanguage('ar');
	} else {
		setLanguage('en');
	}
};

function openNav() {
	document.getElementById("mySidenav").style.width = "100%";
}

function closeNav() {
	document.getElementById("mySidenav").style.width = "0";
}