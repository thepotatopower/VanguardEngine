-- フェイズトランシジョン・ドラゴン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.IsSameZone() and obj.GetNumberOf({q.Location, l.Soul}) > 0
end

function Activation()
	obj.Select({q.Location, l.Soul, q.Count, 1}, p.AddToDrop)
	obj.Store(obj.AddToDrop({q.Location, l.Selected}))
	if obj.Exists({q.Location, l.Stored}) then
		local power = obj.GetOriginalShield({q.Location, l.Stored})
		obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, power, p.UntilEndOfBattle)
	end
end