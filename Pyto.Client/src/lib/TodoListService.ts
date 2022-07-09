import { Readable, writable, Writable } from "svelte/store";
import { addTodo, deleteTodo, getTodoList, updateTodo } from "./api/TodoList";

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

  public async updateTodo(todo: TodoModel) {
    await updateTodo(todo)
  }

  public async addTodo(todo: { content: string }) {
    const createdTodo = await addTodo(todo)
    this._todoList.update(x => [...x, createdTodo])
    await this.fetchTodoList()
  }

  public async deleteTodo(todo: TodoModel) {
    await deleteTodo(todo.id)
    this._todoList.update(x => x.filter(y => y.id !== todo.id))
    await this.fetchTodoList()
  }
}

export default new TodoListService();
