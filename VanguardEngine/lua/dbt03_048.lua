-- 明日へ願いを

function RegisterAbilities()
	-- order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	local param = {q.Location, l.Hand, q.Trigger, tt.Critical, q.Trigger, tt.Front, q.Count, 1}
	if check then return obj.CanCB(1) and obj.CanSB(1) and obj.CanReveal(param) end
	obj.CounterBlast(1)
	obj.SoulBlast(1)
	local revealed = obj.ChooseReveal(param)
	obj.Store(revealed)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher})
end

function Activation()
	if obj.Exists({q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher}) then
		obj.Draw(1)
		obj.RearrangeOnTop({q.Location, l.Stored})
	end
end