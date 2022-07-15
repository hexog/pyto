<script lang="ts">
  export let color: "primary" | "secondary" | "link" = "primary";
  export let isActive = false;

  let classes: string = "";
  $: {
    if (color === "primary") {
      if (isActive) {
        classes = "text-nord-1 bg-nord-8";
      } else {
        classes = "text-nord-1 bg-nord-9 hover:bg-nord-10 active:bg-nord-8";
      }
    }

    if (color === "secondary") {
      classes = "hover:bg-nord-9 active:bg-nord-8";
    }

    if (color === "link") {
      classes = "hover:text-nord-8";
    }
  }
</script>

<div class="inline-block">
  <button
    class="flex justify-center items-center px-3 h-8 rounded-sm transition-colors {classes}"
    on:click|stopPropagation
  >
    <slot />
  </button>
</div>
