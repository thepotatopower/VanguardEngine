-- 暗闇隠し

function RegisterAbilities()
	-- blitz order
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnBlitzOrder)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanAddToDrop({q.Location, l.Bind, q.Other, o.Order, q.Count, 4}) end
	obj.CounterBlast(1)
	obj.ChooseAddToDrop({q.Location, l.Bind, q.Other, o.Order, q.Count, 4})
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.EnemyRC, q.Other, o.Attacking})
end

function Activation()
	if obj.Exists({q.Location, l.EnemyRC, q.Other, o.Attacking}) then
		obj.Select({q.Location, l.PlayerUnits, q.Other, o.Attacked, q.Count, 1})
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 20000, p.UntilEndOfBattle)
	end
end