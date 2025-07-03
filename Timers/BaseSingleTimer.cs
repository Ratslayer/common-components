using System.Threading;
using Cysharp.Threading.Tasks;

namespace BB
{
	public abstract class BaseSingleTimer<TSelf> : ProtectedPooledObject<TSelf>
		where TSelf : BaseSingleTimer<TSelf>, new()
	{
		float _delay;
		CancellationTokenSource _tokenSource;
		protected abstract void Invoke();
		protected void Start(float delay)
		{
			_delay = delay;
			_tokenSource = new();
			Run().Forget();
		}
		async UniTaskVoid Run()
		{
			await UniTask
				.WaitForSeconds(_delay, cancellationToken: _tokenSource.Token)
				.SuppressCancellationThrow();
			if (_tokenSource.IsCancellationRequested)
				return;
			Invoke();
			Dispose();
		}
		public override void Dispose()
		{
			base.Dispose();
			if (_tokenSource is null)
				return;
			_tokenSource.Cancel();
			_tokenSource.Dispose();
			_tokenSource = null;
		}
	}
}
