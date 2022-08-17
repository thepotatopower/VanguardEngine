-- ユースベルク "天壁黎騎"

function RegisterAbilities()
	-- treated as skyfall arms
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetActivation("Cont")
	-- on end of battle
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.RevolDress)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnBattleEndsTrigger")
	ability2.SetCondition("OnBattleEndsCondition")
	ability2.SetActivation("OnBattleEnds")
	ability2.SetProperty(p.NotMandatory)
	-- +5000
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(3)
	ability3.SetTiming(a.Cont)
	ability3.SetLocation(l.RC)
	ability3.SetActivation("Cont2")
end

function Cont()
	obj.AddCardValue({q.Other, o.This}, cs.RegardedAsWhenRodeUpon, "Youthberk \"Skyfall Arms\"", p.Continuous)
end

function OnBattleEndsTrigger()
	return obj.IsAttackingUnit()
end

function OnBattleEndsCondition()
	return obj.CanSuperiorRide({q.Location, l.PlayerHand, q.NameContains, obj.LoadName("RevolForm")})
end

function OnBattleEnds()
	obj.Select({q.Location, l.PlayerHand, q.NameContains, obj.LoadName("RevolForm"), q.Count, 1, q.Min, 0}, Prompt.Ride)
	obj.Store(obj.SuperiorRide({q.Location, l.Selected}))
	obj.AddCardValue({q.Location, l.Stored}, cs.BonusDrive, -2, p.UntilEndOfTurn)
end

function Cont2()
	if obj.GetNumberOf("Filter2", {l.RodeThisTurn}) > 0 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end

function Filter2(snapshot)
	return snapshot.GradeOrGreater(3)
end