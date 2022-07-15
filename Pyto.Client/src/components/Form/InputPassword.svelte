<script lang="ts">
  import { classes } from "./InputCommon";

  export let id: string | null = null;
  export let value: string;

  let input: HTMLInputElement;

  export function focus() {
    input.focus();
  }
</script>

<input type="password" {id} bind:value class={classes} bind:this={input} />
