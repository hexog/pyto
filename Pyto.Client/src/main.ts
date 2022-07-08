import App from './App.svelte'
import { readAccessToken } from './lib/AuthenticationService';

try {
  await readAccessToken(); // set actual authentication state
} catch (e) {}

const app = new App({
  target: document.getElementById('app')
})

export default app
