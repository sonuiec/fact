/** @type {import('tailwindcss').Config} */
module.exports = {
    // ADD THE CSHTML PATHS HERE:
    content: [
        "./Views/**/*.cshtml",        // Most important for MVC
        "./Pages/**/*.cshtml",        // For Razor Pages
        "./Shared/**/*.cshtml",
        "./src/**/*.{js,jsx,ts,tsx}",
    ],
    theme: {
        extend: {
            colors: {
                'primary': '#1214B1',
                'lite-primary': '#DEEBFF',
                'lite-blue': '#60a5fa',
                'secondary': '#03AE52',
                'yellows': '#F6C103',
                'grey': '#838382',
                'blue1': '#0066FF',
                'blue2': '#99C2FF',
                'dark-blue': '#344071',
                'table-blue': '#d8e4f6',
                'lite-head-blue': 'rgba(52, 64, 113,0.8)',
                'table-green': '#BBF7D0',
            }
        },
    },
    plugins: [],
};