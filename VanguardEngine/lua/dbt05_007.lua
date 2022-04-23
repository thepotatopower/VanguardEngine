-- 幽遠なる夜に タマユラ

function RegisterAbilities()
	-- on ride phase
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRidePhase)
	ability1.SetLocation(l.VC)
	ability1.SetTrigger("OnRidePhaseTrigger")
	ability1.SetActivation("OnRidePhase")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCost("OnAttackCost")
	ability2.SetActivation("OnAttack")
	-- glitter
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(3)
	ability3.SetProperty(p.IsGlitter)
end

function OnRidePhaseTrigger()
	return obj.IsPlayerTurn()
end

function OnRidePhase()
	local playerAbility = GiveAbility(GetID(), obj.GetPlayerAbilityID())
	playerAbility.SetDescription(4)
	playerAbility.SetTiming(a.Cont)
	playerAbility.SetActivation("PlayerAbility")
	playerAbility.SetResetTiming(p.UntilEndOfTurn)
end

function PlayerAbility()
	obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusPower, 5000, p.Continuous)
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnAttack()
	local option = 0
	if obj.CanSuperiorCall({q.Location, l.Soul, q.NameContains, obj.LoadName("Ririmi"), q.Count, 1}) and
		obj.CanSuperiorCall({q.Location, l.Soul, q.NameContains, obj.LoadName("Rarami"), q.Count, 1}) then
		option = obj.SelectOption(obj.GetDescription(5), obj.GetDescription(6))
	elseif obj.CanSuperiorCall({q.Location, l.Soul, q.Count, 1}) then
		option = 1
	end
	if option == 1 then
		obj.SuperiorCall({q.Location, l.Soul, q.Count, 1})
	elseif option == 2 then
		obj.SuperiorCall({"Filter1", "Filter2"}, {l.Soul}, 2, 2, "", {}, {})
	end
end

function Filter1(id)
	return obj.NameContains(id, obj.LoadName("Ririmi"))
end

function Filter2(id)
	return obj.NameContains(id, obj.LoadName("Rarami"))
end