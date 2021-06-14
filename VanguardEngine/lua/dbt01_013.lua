-- Dragon Deity King of Resurgence, Dragveda

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnDriveCheck, false, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.OnTriggerZone() and obj.CanStand(1) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Stand(1)
	end
	return 0
end