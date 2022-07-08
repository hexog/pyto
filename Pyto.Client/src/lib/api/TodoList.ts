import { readAccessToken } from "../AuthenticationService"
import type { TodoModel } from "../TodoListService"
import { urlBase } from "./Common"

async function getRequiredHeaders() {
    const token = await readAccessToken()
    return {
        "Authorization": `Bearer ${token}`
    }
}

export async function getTodoList() : Promise<{ todoList: TodoModel[] }> {
    return await fetch(`${urlBase}/todo-list`, {
        method: 'GET',
        headers: {
            ...(await getRequiredHeaders())
        }
    }).then(x => x.json())
}

export async function updateTodo(todo: TodoModel): Promise<TodoModel> {
    return await fetch(`${urlBase}/todo-list`, {
        method: 'PATCH',
        headers: {
            ...await getRequiredHeaders(),
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(todo)
    }).then(x => x.json())
}