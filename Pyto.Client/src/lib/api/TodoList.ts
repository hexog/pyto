import { readAccessToken } from "../AuthenticationService"
import type { TodoModel } from "../TodoListService"
import { urlBase } from "./Common"

async function getRequiredHeaders() {
    const token = await readAccessToken()
    return {
        "Authorization": `Bearer ${token}`
    }
}

const jsonContentHeaders = {
    'Content-Type': 'application/json'
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
    return await fetch(`${urlBase}/todo-list/todos`, {
        method: 'PUT',
        headers: {
            ...await getRequiredHeaders(),
            ...jsonContentHeaders,
        },
        body: JSON.stringify(todo)
    }).then(x => x.json())
}


export async function addTodo(todo: { content: string }): Promise<TodoModel> {
    return await fetch(`${urlBase}/todo-list/todos`, {
        method: 'POST',
        headers: {
            ...await getRequiredHeaders(),
            ...jsonContentHeaders,
        },
        body: JSON.stringify(todo)
    }).then(x => x.json())
}

export async function deleteTodo(id: string): Promise<void> {
    await fetch(`${urlBase}/todo-list/todos/${id}`, {
        method: 'DELETE',
        headers: {
            ...(await getRequiredHeaders())
        }
    })
}