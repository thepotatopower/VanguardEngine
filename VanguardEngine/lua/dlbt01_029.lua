-- Selfie Practice, Anneliese

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Count, 2
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBattleEnds, p.HasPrompt, p.AddToSoul, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsBoosted() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		if obj.Exists(3) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(2, 5000)
	end
	return 0
end