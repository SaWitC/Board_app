export interface AddBoardDTO {
  id: string;
  title: string;
  description: string;
  users: string[];
  admins: string[];
  owners: string[];
}
