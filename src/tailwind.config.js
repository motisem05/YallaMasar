/** @type {import('tailwindcss').Config} */

const colors = require('tailwindcss/colors');

module.exports = {
	darkMode: 'class',
	content: ["./**/*.{html,js,cshtml,razor,cs}"],
	plugins: [
		require('@tailwindcss/aspect-ratio'),
		require('@tailwindcss/line-clamp'),
	],
	theme: {
		extend: {
			colors: {
				primary: colors.emerald,
				accent: colors.red,
			},
			backdropBlur: {
				xs: '2px',
			},
			backdropSaturate: {
				25: '.25',
				40: '.40',
			}
		},
	},
}