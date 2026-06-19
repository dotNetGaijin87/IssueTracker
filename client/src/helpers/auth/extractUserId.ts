function extractUserId(user: string | undefined): string {
  if (user === undefined) {
    new Error('No UserId');
  }
  const userId = user?.split('|').pop();

  if (userId === undefined) {
    new Error('No UserId');
  }
  return userId!;
}

export default extractUserId;
