-- 流麗の美姫 マレイン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function Activation()
	local tempAbility = GiveAbility(GetID(), GetID())
	tempAbility.SetDescription(2)
	tempAbility.SetProperty(p.Powerful)
	tempAbility.SetTiming(a.Cont)
	tempAbility.SetLocation(l.RC)
	tempAbility.SetActivation("TempAbility")
	tempAbility.SetResetTarget(GetID())
	tempAbility.SetResetTiming(p.UntilEndOfTurn)
end

function TempAbility()
	if obj.SoulCount() == 0 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 2000, p.Continuous)
	end
end