-- Cardinal Noid, Cubisia

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PutOnOrderZone, t.Auto, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.Cont, t.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() and obj.LastPutOnOrderZoneIsWorld() then
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

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(1, 5000)
	elseif n == 2 then
		if obj.IsPlayerTurn() and obj.IsAbyssalDarkNight() then
			obj.SetAbilityPower(2, 5000)
		end
	end
	return 0
end