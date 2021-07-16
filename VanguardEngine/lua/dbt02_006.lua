-- Aurora Battle Princess, Perio Turquoise

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerPrisoners, q.Count, 2
	elseif n == 2 then
		return q.Location, l.FrontRow, q.Other, o.This, q.Count, 1
	elseif n == 3 then
		return q.Location, l.LastCalledFromPrison, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 5 then
		return q.Location, l.LastCalledFromPrison
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	elseif n == 2 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.Exists(2) and obj.Exists(3) then
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
end

function Activate(n)
	if n == 1 then
		if obj.IsPlayerTurn() and obj.Exists(1) then
			obj.SetAbilityPower(4, 5000)
		else
			obj.SetAbilityPower(4, 0)
		end
	elseif n == 2 then
		obj.AddTempPower(5, -5000)
	end
	return 0
end
