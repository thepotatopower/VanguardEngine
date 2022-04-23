-- 枢機の主神 オルフィスト・レギス

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetActivation("OnPlace")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetLocation(l.VC)
	ability2.SetTiming(a.OnAttack)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetCost("OnAttackCost")
	ability2.SetActivation("OnAttack")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.Exists({q.Location, l.Hand, q.Location, l.Drop, q.Other, o.World, q.Count, 1})
end

function OnPlace()
	obj.ChoosePutIntoOrderZone({q.Location, l.Hand, q.Location, l.Drop, q.Other, o.World, q.Count, 1, q.Min, 0})
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.IsAbyssalDarkNight()
end

function OnAttackCost(check)
	if check then return obj.CanCB(1) and obj.CanDiscard(1) end
	obj.CounterBlast(1)
	obj.Discard(1)
end

function OnAttack()
	local count = math.floor(obj.GetNumberOf({q.Location, l.Order, q.Other, o.World}) / 2)
	obj.SuperiorCall({}, {}, count, 0, "dbt01_t01", {}, {})
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000 * count, p.UntilEndOfBattle)
end