-- 迫りくる牙 ケイオス

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.VC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCost("ACTCost")
	ability1.SetActivation("ACT")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetActivation("OnAttack")
	-- glitter
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(3)
	ability3.SetProperty(p.IsGlitter)
end

function ACTCost(check)
	if check then return obj.CanCB(1) and obj.CanDiscard(1) end
	obj.CounterBlast(1)
	obj.Discard(1)
end

function ACT()
	obj.LookAtTopOfDeck(3)
	obj.ChooseAddToHand({q.Location, l.Looking, q.Count, 1})
	obj.SuperiorCall({q.Location, l.Looking, q.Other, o.Unit, q.Count, 1})
	obj.AddToSoul({q.Location, l.Looking})
end

function OnAttackTrigger()
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function OnAttackCondition()
	return obj.GetNumberOf({q.Location, l.Soul, q.Other, o.DifferentNames}) >= 8 and obj.Exists({q.Location, l.PlayerRC})
end

function OnAttack()
	local soulCount = obj.GetNumberOf({q.Location, l.Soul, q.Other, o.DifferentNames})
	if soulCount >= 13 then
		obj.AutoSelect({q.Location, l.PlayerRC})
	elseif soulCount >= 8 then
		obj.Select({q.Location, l.PlayerRC, q.Count, 1})
	end
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end