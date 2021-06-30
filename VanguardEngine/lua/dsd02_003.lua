-- Diabolos, "Bad" Steve

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	elseif n == 2 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() and obj.CanSuperiorCall(1, FL.PlayerBackCenter) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() then
			return true
		end
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(1, FL.PlayerBackCenter)
		obj.SoulCharge(1)
	elseif n == 2 then
		if obj.InFinalRush() then
			obj.SetAbilityPower(2, 5000)
		else 
			obj.SetAbilityPower(2, 0)
		end
	end
	return 0
end