namespace BB
{
	public sealed class DisplayErrorsAtMouse : PooledObject<DisplayErrorsAtMouse>, ICostErrorComponent
	{
		public void CostError(in CostBuilderContext context)
		{
			if (World.Has(out MousePointer pointer))
				foreach (var error in context._errors)
					World.RaiseEvent(new ShowHintEvent(error, pointer.Position));
		}
	}

}