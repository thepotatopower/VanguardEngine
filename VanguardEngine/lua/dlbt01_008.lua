-- Rondeau of Dusk Moon, Feltyrosa

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Race, r.Ghost, q.UnitType, u.Normal, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnDriveCheck, p.HasPrompt, p.CB, 1, p.CostNotRequired
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() and obj.CanSuperiorCall(1, FL.FrontRow) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(1, FL.FrontRow)
		if obj.ChoosesToPayCost() then
			obj.AddUntilEndOfBattleValue(2, cs.BonusDrive, 1)
		end
	end
	return 0
end