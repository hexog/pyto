/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{svelte,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        nord: {
          "0": "#2E3440",
          "1": "#3B4252",
          "2": "#434C5E",
          "3": "#4C566A",
          "4": "#D8DEE9",
          "5": "#E5E9F0",
          "6": "#ECEFF4",
          "7": "#8FBCBB",
          "8": "#88C0D0",
          "9": "#81A1C1",
          "10": "#5E81AC",
          "11": "#BF616A",
          "12": "#D08770",
          "13": "#EBCB8B",
          "14": "#A3BE8C",
          "15": "#B48EAD"
        },
        "polar-night": {
          nord0: "#2E3440",
          nord1: "#3B4252",
          nord2: "#434C5E",
          nord3: "#4C566A"
        },
        "snow-storm": {
          nord4: "#D8DEE9",
          nord5: "#E5E9F0",
          nord6: "#ECEFF4"
        },
        frost: {
          nord7: "#8FBCBB",
          nord8: "#88C0D0",
          nord9: "#81A1C1",
          nord10: "#5E81AC"
        },
        aurora: {
          nord11: "#BF616A",
          nord12: "#D08770",
          nord13: "#EBCB8B",
          nord14: "#A3BE8C",
          nord15: "#B48EAD"
        }
      }
    },
  },
  plugins: [],
}
