-- Knight of Heavenly Sword, Rooks

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Grade, 3, q.Count, 3
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Grade, 3, q.Count, 3
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRodeUponThisTurn() and obj.VanguardIs("Apex Ruler, Bastion") and obj.CanReveal(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.ChooseReveal(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.Draw(1)
		obj.OnRideAbilityResolved()
	elseif n == 2 then
		if obj.IsPlayerTurn() and obj.Exists(2) then
			obj.SetAbilityPower(3, 5000)
			obj.AddSkill(3, 0)
		else
			obj.SetAbilityPower(3, 0)
			obj.RemoveSkill(3, 0)
		end
	end
	return 0
end