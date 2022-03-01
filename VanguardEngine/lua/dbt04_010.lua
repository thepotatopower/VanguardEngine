-- 頂を超える剣 バスティオン・プライム

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.VC)
	ability1.SetActivation("Cont")
	-- on battle ends
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.VC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetTrigger("OnBattleEndsTrigger")
	ability2.SetCost("OnBattleEndsCost")
	ability2.SetCanFullyResolve("OnBattleEndsCanFullyResolve")
	ability2.SetActivation("OnBattleEnds")
end

function Cont()
	if obj.IsPlayerTurn() then
		obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusPower, 2000, p.Continuous)
	end
end

function OnBattleEndsTrigger()
	return obj.Exists({q.Location, l.RevealedDriveChecks, q.Grade, 3, q.Other, o.GradeOrHigher})
end

function OnBattleEndsCost(check)
	if check then return obj.CanAddToDrop({q.Location, l.Hand, q.Location, l.Soul, q.NameContains, obj.LoadName("Bastion"), q.Count, 1}) end
	obj.ChooseAddToDrop({q.Location, l.Hand, q.Location, l.Soul, q.NameContains, obj.LoadName("Bastion"), q.Count, 1})
end

function OnBattleEndsCanFullyResolve()
	return obj.Exists({q.Location, l.PlayerRC, q.Grade, 3})
end

function OnBattleEnds()
	obj.Stand({q.Location, l.PlayerRC, q.Grade, 3})
	obj.AddCardValue({q.Location, l.PlayerRC, q.Grade, 3}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end