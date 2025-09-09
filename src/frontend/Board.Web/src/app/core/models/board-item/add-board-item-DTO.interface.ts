export interface AddBoardItemDTO {
  title: string;
  description: string;
  boardColumnId: string;
  priority: number;
  assigneeId: string;
  dueDate: string;
}
