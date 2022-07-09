<script lang="ts">
    export let id: string | null = null
    
    let input: HTMLTextAreaElement

    export function focus() {
        input.focus()
    }
</script>

<input type="text" {id} bind:value={input} class="bg-nord-2 px-3 h-8 rounded-sm shadow focus:outline-none">