import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Task, TaskStatus, TaskPriority, DragDropEvent } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private tasksSubject = new BehaviorSubject<Task[]>([]);
  public tasks$ = this.tasksSubject.asObservable();

  constructor() {
    this.loadInitialTasks();
  }

  private loadInitialTasks(): void {
    const initialTasks: Task[] = [
      {
        id: '1',
        title: 'Создать дизайн главной страницы',
        description: 'Разработать современный дизайн для главной страницы приложения',
        status: TaskStatus.TODO,
        priority: TaskPriority.HIGH,
        assignee: 'Дизайнер',
        createdAt: new Date(),
        updatedAt: new Date(),
        dueDate: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000),
        tags: ['дизайн', 'UI/UX']
      },
      {
        id: '2',
        title: 'Настроить роутинг',
        description: 'Настроить Angular Router для навигации между страницами',
        status: TaskStatus.IN_PROGRESS,
        priority: TaskPriority.MEDIUM,
        assignee: 'Разработчик',
        createdAt: new Date(),
        updatedAt: new Date(),
        dueDate: new Date(Date.now() + 3 * 24 * 60 * 60 * 1000),
        tags: ['разработка', 'Angular']
      },
      {
        id: '3',
        title: 'Тестирование компонентов',
        description: 'Написать unit тесты для всех компонентов',
        status: TaskStatus.REVIEW,
        priority: TaskPriority.HIGH,
        assignee: 'QA',
        createdAt: new Date(),
        updatedAt: new Date(),
        dueDate: new Date(Date.now() + 5 * 24 * 60 * 60 * 1000),
        tags: ['тестирование', 'качество']
      },
      {
        id: '4',
        title: 'Тестирование компонентов',
        description: 'Написать unit тесты для всех компонентов',
        status: TaskStatus.REVIEW,
        priority: TaskPriority.HIGH,
        assignee: 'QA',
        createdAt: new Date(),
        updatedAt: new Date(),
        dueDate: new Date(Date.now() + 5 * 24 * 60 * 60 * 1000),
        tags: ['тестирование', 'качество']
      }
    ];
    this.tasksSubject.next(initialTasks);
  }

  getTasks(): Observable<Task[]> {
    return this.tasks$;
  }

  getTasksByStatus(status: TaskStatus): Observable<Task[]> {
    return new Observable(observer => {
      this.tasks$.subscribe(tasks => {
        const filteredTasks = tasks.filter(task => task.status === status);
        observer.next(filteredTasks);
      });
    });
  }

  getTaskById(id: string): Task | undefined {
    return this.tasksSubject.value.find(task => task.id === id);
  }

  createTask(taskData: Omit<Task, 'id' | 'createdAt' | 'updatedAt'>): Task {
    const newTask: Task = {
      ...taskData,
      id: this.generateId(),
      createdAt: new Date(),
      updatedAt: new Date()
    };

    const currentTasks = this.tasksSubject.value;
    this.tasksSubject.next([...currentTasks, newTask]);
    return newTask;
  }

  updateTask(id: string, updates: Partial<Task>): Task | null {
    const currentTasks = this.tasksSubject.value;
    const taskIndex = currentTasks.findIndex(task => task.id === id);

    if (taskIndex === -1) {
      return null;
    }

    const updatedTask = {
      ...currentTasks[taskIndex],
      ...updates,
      updatedAt: new Date()
    };

    const newTasks = [...currentTasks];
    newTasks[taskIndex] = updatedTask;
    this.tasksSubject.next(newTasks);

    return updatedTask;
  }

  moveTask(taskId: string, newStatus: TaskStatus): boolean {
    const task = this.getTaskById(taskId);
    if (!task) {
      return false;
    }

    this.updateTask(taskId, { status: newStatus });
    return true;
  }

  moveTaskByDragDrop(event: DragDropEvent): boolean {
    const task = this.getTaskById(event.taskId);
    if (!task) {
      return false;
    }

    // Проверяем, что статус действительно изменился
    if (event.fromStatus === event.toStatus) {
      return false;
    }

    this.updateTask(event.taskId, { status: event.toStatus });
    return true;
  }

  deleteTask(id: string): boolean {
    const currentTasks = this.tasksSubject.value;
    const taskIndex = currentTasks.findIndex(task => task.id === id);

    if (taskIndex === -1) {
      return false;
    }

    const newTasks = currentTasks.filter(task => task.id !== id);
    this.tasksSubject.next(newTasks);
    return true;
  }

  private generateId(): string {
    return Date.now().toString(36) + Math.random().toString(36).substr(2);
  }
}
