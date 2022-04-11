-- 双翼の大天使 アレスティエル

function RegisterAbilities()
	-- on main phase
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnMainPhase)
	ability1.SetLocation(l.VC)
	ability1.SetTrigger("OnMainPhaseTrigger")
	ability1.SetCondition("OnMainPhaseCondition")
	ability1.SetActivation("OnMainPhase")
	-- white wings
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.WhiteWings)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.VC)
	ability2.SetActivation("WhiteWings")
	-- black wings
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(3)
	ability3.SetProperty(p.BlackWings)
	ability3.SetTiming(a.Cont)
	ability3.SetLocation(l.VC)
	ability3.SetActivation("BlackWings")
end

function OnMainPhaseTrigger()
	return obj.IsPlayerTurn()
end

function OnMainPhaseCondition()
	return obj.GetNumberOf("", {l.Bind}) > 0
end

function OnMainPhase()
	obj.Select("", {l.Bind}, 1, 1, {}, Prompt.AddToHand)
	obj.Store(obj.AddToHand({q.Location, l.Selected}))
	if obj.GetNumberOf({q.Location, l.Stored}) > 0 then
		obj.BindTopOfDeck(1)
	end
end

function WhiteWings()
	if obj.IsPlayerTurn() then
		obj.AddCardValue({q.Location, l.PlayerVC}, cs.BonusPower, 5000, p.Continuous)
		obj.AddCardValue({q.Location, l.PlayerVC}, cs.BonusCritical, 1, p.Continuous)
	end
end

function BlackWings()
	if obj.IsPlayerTurn() then
		obj.AddCardValue({q.Location, l.EnemyUnits}, cs.BonusPower, -5000, p.Continuous)
	end
end