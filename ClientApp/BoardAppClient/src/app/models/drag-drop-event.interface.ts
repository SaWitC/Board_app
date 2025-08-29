import { TaskStatus } from "../enums/task-status.enum";

export interface DragDropEvent {
  taskId: string;
  fromStatus: TaskStatus;
  toStatus: TaskStatus;
}
