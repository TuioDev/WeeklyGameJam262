public class ToDoItem
{
    public string Identifier { get; private set; }
    public string Text { get; private set; }
    public bool IsDone { get; private set; }

    public ToDoItem(string identifier, string text)
    {
        Identifier = identifier;
        Text = text;
        IsDone = false;
    }

    public ToDoItem(string identifier, string text, bool isDone)
    {
        Identifier = identifier;
        Text = text;
        IsDone = isDone;
    }

    public void Done()
    {
        IsDone = true;
    }
}