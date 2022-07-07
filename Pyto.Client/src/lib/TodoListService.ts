import { Readable, writable, Writable } from "svelte/store";
import { getTodoList, updateTodo } from "./api/TodoList";

export type TodoState = "checked" | "unchecked";

export type TodoModel = {
  id: string;
  content: string;
  state: TodoState;
};

export class TodoListService {
  private _todoList: Writable<TodoModel[]> = writable([]);
  public get todoList(): Readable<TodoModel[]> {
    return this._todoList;
  }

  constructor() {
    this.fetchTodoList()    
  }

  public async fetchTodoList() {
    const response = await getTodoList();
    this._todoList.set(response.todoList);
  }

  public async updateTodo(todo: TodoModel)  {
    await updateTodo(todo)
  }
}

export default new TodoListService();
