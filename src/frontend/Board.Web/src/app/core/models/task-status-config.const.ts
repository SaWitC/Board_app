import { TaskStatus } from "../enums/task-status.enum";
import { TaskStatusConfig } from "./task-status-config.interface";

export const TASK_STATUS_CONFIG: TaskStatusConfig[] = [
  {
    status: TaskStatus.TODO,
    label: 'К выполнению',
    color: '#e3f2fd',
    icon: 'assignment'
  },
  {
    status: TaskStatus.IN_PROGRESS,
    label: 'В работе',
    color: '#fff3e0',
    icon: 'play_arrow'
  },
  {
    status: TaskStatus.REVIEW,
    label: 'На проверке',
    color: '#f3e5f5',
    icon: 'visibility'
  },
  {
    status: TaskStatus.DONE,
    label: 'Завершено',
    color: '#e8f5e8',
    icon: 'check_circle'
  }
];
