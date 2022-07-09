<script lang="ts">
    export let value = ''
    let input: HTMLTextAreaElement

    export function focus() {
        input.focus()
    }
</script>

<textarea bind:value={value} class="bg-nord-2 px-3 h-8 rounded-sm shadow focus:outline-none" bind:this={input}></textarea>