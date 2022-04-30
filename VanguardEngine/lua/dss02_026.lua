-- 泰然の天断 ヒースコート

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Cont")
	-- on retire
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnRetire)
	ability2.SetTrigger("Trigger")
	ability2.SetCost("Cost")
	ability2.SetCanFullyResolve("CanFullyResolve")
	ability2.SetActivation("Activation")
end

function Cont()
	if obj.GetNumberOf("Filter1", {l.RevealedDriveChecksThisTurn}) > 0 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end

function Filter1(snapshot)
	return snapshot.grade == 3
end

function Trigger()
	return obj.IsApplicable() and not obj.IsPlayerTurn()
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.GetNumberOf("Filter2", {l.Drop}) > 0
end

function Activation()
	obj.Select("Filter2", {l.Drop}, 1, 1, Prompt.AddToHand)
	obj.AddToHand({q.Location, l.Selected})
end

function Filter2(id)
	return not obj.NameIs(id, obj.GetNameFromCardID("dss02_026"))
end