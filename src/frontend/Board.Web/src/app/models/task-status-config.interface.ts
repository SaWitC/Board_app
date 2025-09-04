import { TaskStatus } from "../enums/task-status.enum";

export interface TaskStatusConfig {
  status: TaskStatus;
  label: string;
  color: string;
  icon: string;
}
