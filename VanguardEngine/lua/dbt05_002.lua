-- バーニングホーン・ドラゴン

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCost("OnPlaceCost")
	ability1.SetActivation("OnPlace")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetActivation("OnAttack")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnPlace()
	obj.LookAtTopOfDeck(7)
	obj.Store(obj.Search({q.Location, l.Looking, q.NameContains, obj.LoadName("Overlord"), q.Count, 1, q.Min, 0}))
	if not obj.Exists({q.Location, l.Stored}) then
		obj.CounterCharge(1)
	end
end

function OnAttackTrigger()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Overlord"), q.Other, o.Attacking})
end

function OnAttackCondition()
	return obj.IsSameZone()
end

function OnAttack()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end