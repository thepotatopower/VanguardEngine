-- デザイアデビル ドフンド

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetMovedFrom(l.Hand)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetActivation("OnPlace")
	ability1.SetProperty(p.NotMandatory)
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetCost("OnAttackCost")
	ability1.SetCanFullyResolve("OnAttackCanFullyResolve")
	ability2.SetActivation("OnAttack")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt03_003")})
end

function OnPlace()
	obj.SoulCharge(2)
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.GetNumberOf({q.Location, l.Soul, q.Name, obj.GetNameFromCardID("dbt03_003")}) >= 2
end

function OnAttackCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnAttackCanFullyResolve()
	return obj.IsSameZone()
end

function OnAttack()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusCritical, 1, p.UntilEndOfBattle)
end