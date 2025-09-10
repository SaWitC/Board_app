import { TaskType } from "../enums/task-type.enum";

export interface UpdateBoardItemDTO {
  id: string;
  title: string;
  description: string;
  boardColumnId: string;
  priority: number;
  assigneeId: string;
  dueDate: string;
  taskType: TaskType;
}
