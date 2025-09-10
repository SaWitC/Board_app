export interface UpdateBoardDTO {
  id: string;
  title: string;
  description: string;
  boardUsers: { email: string; role: number; }[];
  boardColumns: { id?: string; title: string; description: string; }[];
}
